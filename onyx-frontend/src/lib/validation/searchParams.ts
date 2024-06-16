import { z } from "zod";
import { MonthStringSchema, YearStringSchema } from "@/lib/validation/base";

export const SingleBudgetPageParamsSchema = z.object({
  selectedBudget: z.string(),
  month: MonthStringSchema,
  year: YearStringSchema,
});

export const LayoutSearchParamsSchema = z.object({
  selectedBudget: z.string().optional(),
});
