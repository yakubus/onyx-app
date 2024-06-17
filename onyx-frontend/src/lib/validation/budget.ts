import { z } from "zod";
import { ResultSchema } from "@/lib/validation/base";
import { AccountSchema } from "@/lib/validation/account";
import { CategorySchema } from "@/lib/validation/category";
import { CounterpartySchema } from "@/lib/validation/counterparty";
import { CURRENCY } from "@/lib/constants/currency";

const currencyValues = CURRENCY.map((c) => c.value) as [string, ...string[]];

export const BudgetSchema = z.object({
  id: z.string(),
  name: z.string(),
  currency: z.string(),
  userIds: z.array(z.string()),
  optimistic: z.boolean().optional(),
});

export const BudgetResultSchema = ResultSchema.extend({
  value: z.array(BudgetSchema),
});

export const CreateBudgetSchema = z.object({
  name: z
    .string()
    .min(1, "Please provide budget name.")
    .regex(/^[a-zA-Z0-9\s.-]{1,50}$/, "Invalid budget name."),
  currency: z.enum(currencyValues, { message: "Invalid currency" }),
  userId: z.string(),
});
export type CreateBudget = z.infer<typeof CreateBudgetSchema>;

export type Budget = z.infer<typeof BudgetSchema>;

export const BudgetWithPayloadSchema = BudgetSchema.extend({
  accounts: z.array(AccountSchema),
  categories: z.array(CategorySchema),
  counterparties: z.array(CounterpartySchema),
});

export type BudgetWithPayload = z.infer<typeof BudgetWithPayloadSchema>;

export const BudgetWithPayloadResultSchema = ResultSchema.extend({
  value: BudgetWithPayloadSchema,
});
