import { z } from "zod";
import { MonthStringSchema, YearStringSchema } from "@/lib/validation/base";

export const SingleBudgetPageParamsSchema = z.object({
  month: MonthStringSchema,
  year: YearStringSchema,
  accMonth: MonthStringSchema,
  accYear: YearStringSchema,
  selectedAcc: z.string().optional(),
});

export type SingleBudgetPageSearchParams = z.infer<
  typeof SingleBudgetPageParamsSchema
>;
