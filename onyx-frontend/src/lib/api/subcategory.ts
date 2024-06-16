import { privateApi } from "@/lib/axios";
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

export interface FormAssignment {
  assignedAmount: number;
  assignmentMonth: MonthDate;
}

interface Assign {
  budgetId: string;
  subcategoryId: string;
  assignment: FormAssignment;
}

export interface FormTarget {
  targetAmount: number;
  startedAt: MonthDate;
  targetUpToMonth: MonthDate;
}

export interface CreateTargetForm {
  budgetId: string;
  subcategoryId: string;
  formTarget: FormTarget;
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

export const assign = ({ budgetId, subcategoryId, assignment }: Assign) =>
  privateApi.put(
    `/${budgetId}/subcategories/${subcategoryId}/assignment`,
    assignment,
  );

export const createTarget = ({
  budgetId,
  subcategoryId,
  formTarget,
}: CreateTargetForm) =>
  privateApi.put(
    `/${budgetId}/subcategories/${subcategoryId}/target`,
    formTarget,
  );
