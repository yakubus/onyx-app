import { z } from "zod";
import { RequiredString } from "@/lib/validation/base";

export const CounterpartySchema = z.object({
  id: RequiredString,
  name: RequiredString,
  type: z.enum(["Payee", "Payer"]),
});

export type Counterparty = z.infer<typeof CounterpartySchema>;
