import { z } from "zod";
import { AssignmentSchema, TargetSchema } from "@/lib/validation/base";

export const SubcategorySchema = z.object({
  id: z.string().min(1),
  name: z.string().min(1),
  description: z.string().optional().nullish(),
  assigments: z.array(AssignmentSchema).optional(),
  target: TargetSchema.optional().nullish(),
});
export type Subcategory = z.infer<typeof SubcategorySchema>;
