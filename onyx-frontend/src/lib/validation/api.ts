import { z } from "zod";

const ErrorSchema = z.object({
  code: z.string(),
  message: z.string(),
});

const MoneySchema = z.object({
  currency: z.string().min(1),
  amount: z.number().min(1),
});

const MonthDateSchema = z.object({
  month: z.number(),
  year: z.number(),
});

const ResultSchema = z.object({
  isSuccess: z.boolean(),
  isFailure: z.boolean(),
  error: ErrorSchema,
});

const AccountSchema = z.object({
  id: z.string().min(1),
  name: z.string().min(1),
  balance: MoneySchema,
  type: z.enum(["Checking", "Saving"]),
});

export const AccountResultSchema = ResultSchema.extend({
  value: z.array(AccountSchema),
});
export type Account = z.infer<typeof AccountSchema>;

const TargetSchema = z.object({
  upToMonth: MonthDateSchema,
  startedAt: MonthDateSchema,
  targetAmount: MoneySchema,
  collectedAmount: MoneySchema,
  amountAssignedEveryMonth: MoneySchema,
});

const AssignmentSchema = z.object({
  month: MonthDateSchema,
  assignedAmount: MoneySchema,
  actualAmount: MoneySchema,
});

const SubcategorySchema = z.object({
  id: z.string().min(1),
  name: z.string().min(1),
  description: z.string().optional().nullish(),
  assigments: z.array(AssignmentSchema).optional(),
  target: TargetSchema.optional().nullish(),
});
export type Subcategory = z.infer<typeof SubcategorySchema>;

export const CategorySchema = z.object({
  id: z.string().min(1),
  name: z.string().min(1),
  subcategories: z.array(SubcategorySchema),
});

export const CategoryResultSchema = ResultSchema.extend({
  value: z.array(CategorySchema),
});
export type Category = z.infer<typeof CategorySchema>;

const Counterparty = z.object({
  id: z.string().min(1),
  name: z.string().min(1),
  type: z.enum(["Payee, Payer"]),
});

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
