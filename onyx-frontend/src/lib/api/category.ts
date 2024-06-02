import axios from "axios";
import { queryOptions } from "@tanstack/react-query";

import { CategoryResultSchema } from "@/lib/validation/category";
import { getErrorMessage } from "@/lib/utils";

export const getCategories = async () => {
  try {
    const { data } = await axios.get("/api/categories");
    const result = CategoryResultSchema.safeParse(data);

    if (!result.success) {
      throw new Error("Validation failed.");
    }

    const { value, isFailure, error } = result.data;
    if (isFailure) {
      throw new Error(error.message);
    }

    return value;
  } catch (error) {
    throw new Error(getErrorMessage(error));
  }
};

export const getCategoriesQueryOptions = queryOptions({
  queryKey: ["categories"],
  queryFn: getCategories,
});

export const createCategory = (name: string) =>
  axios.post("/api/categories", { name });

export const deleteCategory = (id: string) =>
  axios.delete(`/api/categories/${id}`);

export const editCategoryName = ({
  id,
  newName,
}: {
  id: string;
  newName: string;
}) => axios.put(`/api/categories/${id}`, { newName });
