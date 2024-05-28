import axios from "axios";
import { queryOptions } from "@tanstack/react-query";

import { CategoryResultSchema } from "./validation/api";
import { getErrorMessage } from "./utils";

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
