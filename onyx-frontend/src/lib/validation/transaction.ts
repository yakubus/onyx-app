import * as z from "zod";

import { SubcategorySchema } from "@/lib/validation/subcategory";
import {
  MoneySchema,
  RequiredString,
  ResultSchema,
} from "@/lib/validation/base";
import { CounterpartySchema } from "@/lib/validation/counterparty";
import { AccountSchema } from "@/lib/validation/account";

export const TransactionSchema = z.object({
  id: RequiredString,
  subcategory: SubcategorySchema.nullable(),
  amount: MoneySchema,
  originalAmount: MoneySchema.optional(),
  account: AccountSchema,
  counterparty: CounterpartySchema,
  transactedAt: RequiredString,
  optimistic: z.boolean().optional(),
});
export type Transaction = z.infer<typeof TransactionSchema>;
export const TransactionResultSchema = ResultSchema.extend({
  value: z.array(TransactionSchema),
});

export const CreateTransactionSchema = z.object({
  counterpartyName: RequiredString.max(
    50,
    "Max length of counterparty name is 50 characters.",
  ),
  currency: RequiredString,
  amount: RequiredString.refine((v) => parseFloat(v) !== 0, {
    message: "Amount cannot be 0.",
  }),
  transactedAt: z.date(),
  subcategoryId: RequiredString,
  categoryId: RequiredString,
});

export type TCreateTransactionSchema = z.infer<typeof CreateTransactionSchema>;
