import { queryOptions } from "@tanstack/react-query";

import { privateApi } from "@/lib/axios";
import { getErrorMessage } from "@/lib/utils";
import { CategoryResultSchema } from "@/lib/validation/category";

export const getCategories = async (budgetId: string) => {
  try {
    const { data } = await privateApi.get(`/${budgetId}/categories`);
    const validatedData = CategoryResultSchema.safeParse(data);
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

export const getCategoriesQueryOptions = (budgetId: string) =>
  queryOptions({
    queryKey: ["categories", budgetId],
    queryFn: () => getCategories(budgetId),
  });

export const createCategory = ({
  budgetId,
  name,
}: {
  budgetId: string;
  name: string;
}) => privateApi.post(`/${budgetId}/categories`, { name });

export const deleteCategory = ({
  budgetId,
  categoryId,
}: {
  budgetId: string;
  categoryId: string;
}) => privateApi.delete(`/${budgetId}/categories/${categoryId}`);

export const editCategoryName = ({
  budgetId,
  categoryId,
  newName,
}: {
  budgetId: string;
  categoryId: string;
  newName: string;
}) => privateApi.put(`${budgetId}/categories/${categoryId}`, { newName });
