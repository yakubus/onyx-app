export const TransactionSchema = z.object({
  id: z.string().min(1),
  subcategory: SubcategorySchema.optional(),
  amount: MoneySchema,
  originalAmount: MoneySchema.optional(),
  account: AccountSchema,
  counterParty: Counterparty,
  transactedAt: z.date(),
});
export type Transaction = z.infer<typeof TransactionSchema>;
export const TransactionResultSchema = ResultSchema.extend({
  value: z.array(TransactionSchema),
});
