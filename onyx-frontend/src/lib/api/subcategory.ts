import { privateApi } from "../axios";

interface CreateSubcategory {
  budgetId: string;
  parentCategoryId: string;
  subcategoryName: string;
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
