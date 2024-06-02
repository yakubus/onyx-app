import * as z from "zod";

import { SubcategorySchema } from "@/lib/validation/subcategory";
import { MoneySchema, ResultSchema } from "@/lib/validation/base";
import { CounterpartySchema } from "@/lib/validation/counterparty";
import { AccountSchema } from "@/lib/validation/account";

export const TransactionSchema = z.object({
  id: z.string().min(1),
  subcategory: SubcategorySchema.optional(),
  amount: MoneySchema,
  originalAmount: MoneySchema.optional(),
  account: AccountSchema,
  counterParty: CounterpartySchema,
  transactedAt: z.date(),
});
export type Transaction = z.infer<typeof TransactionSchema>;
export const TransactionResultSchema = ResultSchema.extend({
  value: z.array(TransactionSchema),
});
