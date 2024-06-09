import { z } from "zod";
import { AssignmentSchema, TargetSchema } from "@/lib/validation/base";

export const SubcategorySchema = z.object({
  id: z.string().min(1),
  name: z.string().min(1),
  description: z.string().optional().nullish(),
  assigments: z.array(AssignmentSchema).optional(),
  target: TargetSchema.optional().nullish(),
  optimistic: z.boolean().optional(),
});
export type Subcategory = z.infer<typeof SubcategorySchema>;

export const CreateSubcategorySchema = z.object({
  name: z
    .string()
    .min(1, "Please provide subcategory name.")
    .regex(/^[a-zA-Z0-9\s.-]{1,50}$/, "Invalid subcategory name."),
});

export type CreateSubcategory = z.infer<typeof CreateSubcategorySchema>;
