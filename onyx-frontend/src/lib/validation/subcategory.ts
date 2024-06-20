import { z } from "zod";
import {
  AssignmentSchema,
  MoneySchema,
  MonthStringSchema,
  ResultSchema,
  TargetSchema,
  YearStringSchema,
} from "@/lib/validation/base";

export const SubcategorySchema = z.object({
  id: z.string().min(1),
  name: z.string().min(1),
  description: z.string().nullable(),
  assignments: z.array(AssignmentSchema).nullable(),
  target: TargetSchema.nullable(),
  optimistic: z.boolean().optional(),
});
export type Subcategory = z.infer<typeof SubcategorySchema>;

export const CreateSubcategorySchema = z.object({
  name: z
    .string()
    .min(1, "Please provide subcategory name.")
    .regex(/^[a-zA-Z0-9\s.-]{1,50}$/, "Invalid subcategory name."),
});

export type CreateSubcategory = z.infer<typeof CreateSubcategorySchema>;

const AmountSchema = z
  .string()
  .min(1, "Please provide amount.")
  .regex(/^(0|[1-9]\d*)(\.\d{1,2})?$/, "Invalid amount.");

export const ToAssignSchema = ResultSchema.extend({
  value: MoneySchema,
});

export const CreateTargetSchema = z.object({
  amount: AmountSchema,
  year: YearStringSchema,
  month: MonthStringSchema,
});

export type CreateTarget = z.infer<typeof CreateTargetSchema>;

export const CreateDescriptionSchema = z.object({
  description: z
    .string()
    .max(255, "Maximum length of description is 255 characters."),
});

export type CreateDescription = z.infer<typeof CreateDescriptionSchema>;

export const assignmentLiveValidation = (value: string) => {
  // Replace empty input with '0'
  if (value === "") {
    value = "0";
  }

  // Remove any non-numeric characters except '.'
  value = value.replace(/[^\d.]/g, "");

  // Remove leading zeros before the whole number part
  value = value.replace(/^0+(?=\d)/, "");

  // Remove leading zero if it's the only digit before a decimal point
  value = value.replace(/(^|-)0+(\d+\.\d*)/, "$1$2");

  // Remove leading zero if it's the only digit
  value = value.replace(/^0+(?=\d)/, "");

  // Replace multiple dots with a single dot
  value = value.replace(/(\..*)\./g, "$1");

  // Limit decimal places to 2 digits if there is a decimal part
  const parts = value.split(".");
  if (parts.length > 1) {
    value = `${parts[0].slice(0, 9)}.${parts[1].slice(0, 2)}`; // Limit to 9 digits before decimal and 2 digits after
  } else {
    value = value.slice(0, 9); // Limit to 9 digits if there's no decimal part
  }

  return value;
};
