import { queryOptions } from "@tanstack/react-query";

import { privateApi } from "@/lib/axios";
import { getErrorMessage } from "@/lib/utils";
import {
  BudgetResultSchema,
  BudgetWithPayloadResultSchema,
} from "@/lib/validation/budget";
import { ToAssignSchema } from "../validation/subcategory";

interface GetToAssign {
  month: string;
  year: string;
  budgetId: string;
}

const getBudgets = async () => {
  try {
    const { data } = await privateApi.get("/budgets");
    const validatedData = BudgetResultSchema.safeParse(data);

    if (!validatedData.success) {
      throw new Error("Invalid data type.");
    }

    const { value, isFailure, error } = validatedData.data;
    if (isFailure) {
      throw new Error(error.message);
    }

    return value;
  } catch (error) {
    throw new Error(getErrorMessage(error));
  }
};

export const getBudgetsQueryOptions = queryOptions({
  queryKey: ["budgets"],
  queryFn: getBudgets,
});

export const createBudget = (budgetName: string) =>
  privateApi.post("/budgets", { budgetName });

export const deleteBudget = (id: string) => privateApi.delete(`/budgets/${id}`);

export const getBudget = async (id: string) => {
  try {
    const { data } = await privateApi.get(`/budgets/${id}`);
    const validatedData = BudgetWithPayloadResultSchema.safeParse(data);

    if (!validatedData.success) {
      console.log(validatedData.error.flatten());
      throw new Error("Invalid data type.");
    }

    const { value, isFailure, error } = validatedData.data;
    if (isFailure) {
      throw new Error(error.message);
    }

    return value;
  } catch (error) {
    console.error(getErrorMessage(error));
    throw new Error(getErrorMessage(error));
  }
};

export const getBudgetQueryOptions = (id: string) =>
  queryOptions({
    queryKey: ["budget", id],
    queryFn: () => getBudget(id),
  });

export const getToAssign = async ({ month, year, budgetId }: GetToAssign) => {
  try {
    const { data } = await privateApi.get(
      `/${budgetId}/subcategories/to-assign?month=${month}&year=${year}`,
    );
    const validatedData = ToAssignSchema.safeParse(data);
    if (!validatedData.success) {
      console.log(validatedData.error.flatten());
      throw new Error("Invalid data type.");
    }

    const { value, isFailure, error } = validatedData.data;
    if (isFailure) {
      throw new Error(error.message);
    }

    return value;
  } catch (error) {
    console.error(getErrorMessage(error));
    throw new Error(getErrorMessage(error));
  }
};

export const getToAssignQueryKey = (budgetId: string) => ["toAssign", budgetId];

export const getToAssignQueryOptions = ({
  month,
  year,
  budgetId,
}: GetToAssign) =>
  queryOptions({
    queryKey: ["toAssign", budgetId],
    queryFn: () => getToAssign({ month, year, budgetId }),
  });
