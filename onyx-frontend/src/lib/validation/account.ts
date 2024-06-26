import { z } from "zod";
import {
  AccountTypeSchema,
  MoneySchema,
  NameSchema,
  RequiredString,
  ResultSchema,
} from "@/lib/validation/base";

export const AccountSchema = z.object({
  id: z.string().min(1),
  name: z.string().min(1),
  balance: MoneySchema,
  type: AccountTypeSchema,
  optimistic: z.boolean().optional(),
});

export const AccountResultSchema = ResultSchema.extend({
  value: z.array(AccountSchema),
});

export type Account = z.infer<typeof AccountSchema>;

export const CreateAccountSchema = z.object({
  name: NameSchema,
  amount: RequiredString,
  currency: RequiredString,
  accountType: AccountTypeSchema,
});

export type TCreateAccountForm = z.infer<typeof CreateAccountSchema>;
