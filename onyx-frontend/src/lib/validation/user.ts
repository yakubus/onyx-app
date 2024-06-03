import { z } from "zod";
import { ResultSchema } from "./base";

export const UserSchema = z.object({
  id: z.string(),
  username: z.string(),
  email: z.string().email(),
  currency: z.string(),
  budgetIds: z.array(z.string()),
});

export type User = z.infer<typeof UserSchema>;

export const UserWithTokenSchema = UserSchema.extend({
  accessToken: z.string(),
});
export type UserWithToken = z.infer<typeof UserSchema>;

export const UserWithTokenResultSchema = ResultSchema.extend({
  value: UserWithTokenSchema,
});

export type UserWithTokenResult = z.infer<typeof UserWithTokenResultSchema>;
