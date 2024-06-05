import { queryOptions } from "@tanstack/react-query";

import { privateApi } from "@/lib/axios";
import { getErrorMessage } from "@/lib/utils";
import { BudgetResultSchema } from "@/lib/validation/budget";

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
