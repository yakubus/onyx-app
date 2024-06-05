import { z } from "zod";
import { ResultSchema } from "@/lib/validation/base";

export const BudgetSchema = z.object({
  id: z.string(),
  name: z.string(),
  currency: z.string(),
  userIds: z.array(z.string()),
});

export const BudgetResultSchema = ResultSchema.extend({
  value: z.array(BudgetSchema),
});

export const CreateBudgetSchema = z.object({
  name: z
    .string()
    .min(1, "Please provide budget name.")
    .regex(/^[a-zA-Z0-9\s.-]{1,50}$/, "Invalid budget name."),
});
export type CreateBudget = z.infer<typeof CreateBudgetSchema>;

export type Budget = z.infer<typeof BudgetSchema>;
