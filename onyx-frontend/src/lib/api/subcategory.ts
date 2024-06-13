import { queryOptions } from "@tanstack/react-query";
import { privateApi } from "@/lib/axios";
import { getErrorMessage } from "@/lib/utils";
import { ToAssignSchema } from "@/lib/validation/subcategory";
import { MonthDate } from "@/lib/validation/base";

interface CreateSubcategory {
  budgetId: string;
  parentCategoryId: string;
  subcategoryName: string;
}

interface EditSubcategoryName {
  budgetId: string;
  subcategoryId: string;
  subcategoryName: string;
}

interface GetToAssign {
  month: string;
  year: string;
  budgetId: string;
}

export interface FormAssignment {
  assignedAmount: number;
  assignmentMonth: MonthDate;
}

interface Assign {
  budgetId: string;
  subcategoryId: string;
  assignment: FormAssignment;
}

export const createSubcategory = ({
  budgetId,
  parentCategoryId,
  subcategoryName,
}: CreateSubcategory) =>
  privateApi.post(`/${budgetId}/subcategories`, {
    parentCategoryId,
    subcategoryName,
  });

export const editSubcategoryName = ({
  budgetId,
  subcategoryId,
  subcategoryName,
}: EditSubcategoryName) =>
  privateApi.put(`/${budgetId}/subcategories/${subcategoryId}`, {
    newName: subcategoryName,
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

export const getToAssignQueryOptions = ({
  month,
  year,
  budgetId,
}: GetToAssign) =>
  queryOptions({
    queryKey: ["toAssign", budgetId],
    queryFn: () => getToAssign({ month, year, budgetId }),
  });

export const assign = ({ budgetId, subcategoryId, assignment }: Assign) =>
  privateApi.put(
    `/${budgetId}/subcategories/${subcategoryId}/assignment`,
    assignment,
  );
