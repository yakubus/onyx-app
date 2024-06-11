import { privateApi } from "../axios";

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
