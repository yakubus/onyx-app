import { FC } from "react";
import { SubmitHandler, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";

import {
  HoverCard,
  HoverCardContent,
  HoverCardTrigger,
} from "@/components/ui/hover-card";
import { Button } from "@/components/ui/button";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormMessage,
} from "@/components/ui/form";

import {
  CreateSubcategory,
  CreateSubcategorySchema,
  Subcategory,
} from "@/lib/validation/subcategory";
import { Input } from "@/components/ui/input";
import { Edit } from "lucide-react";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { editSubcategoryName } from "@/lib/api/subcategory";
import { getCategoriesQueryOptions } from "@/lib/api/category";
import { useParams } from "@tanstack/react-router";
import { capitalize, cn } from "@/lib/utils";

interface NameTooltipProps {
  isNameTooltipOpen: boolean;
  setIsNameTooltipOpen: (state: boolean) => void;
  subcategory: Subcategory;
}

const NameTooltip: FC<NameTooltipProps> = ({
  isNameTooltipOpen,
  setIsNameTooltipOpen,
  subcategory,
}) => {
  const queryClient = useQueryClient();
  const { budgetId } = useParams({
    from: "/_dashboard-layout/budget/$budgetId",
  });
  const form = useForm<CreateSubcategory>({
    defaultValues: {
      name: subcategory.name,
    },
    resolver: zodResolver(CreateSubcategorySchema),
  });
  const { handleSubmit, control, setError } = form;

  const { mutate, isPending, variables } = useMutation({
    mutationFn: editSubcategoryName,
    onSettled: async () => {
      return await queryClient.invalidateQueries(
        getCategoriesQueryOptions(budgetId),
      );
    },
    onError: () => {
      setError("name", { message: "Error occured. Try again." });
    },
  });

  const onSubmit: SubmitHandler<CreateSubcategory> = (data) => {
    const { name: subcategoryName } = data;
    mutate({ budgetId, subcategoryId: subcategory.id, subcategoryName });
  };

  return (
    <HoverCard open={isNameTooltipOpen} onOpenChange={setIsNameTooltipOpen}>
      <HoverCardTrigger>
        <span className={cn(isPending && "opacity-50")}>
          {isPending ? capitalize(variables.subcategoryName) : subcategory.name}
        </span>
      </HoverCardTrigger>
      <HoverCardContent>
        <Form {...form}>
          <form
            onSubmit={handleSubmit(onSubmit)}
            className="flex items-center px-1 py-2"
          >
            <FormField
              control={control}
              name="name"
              disabled={isPending}
              render={({ field }) => (
                <FormItem className="w-full space-y-1">
                  <div className="flex space-x-2">
                    <FormControl>
                      <Input {...field} />
                    </FormControl>
                    <Button
                      type="submit"
                      variant="ghost"
                      size="icon"
                      className="w-12 rounded-lg"
                    >
                      <Edit className="opacity-70" />
                    </Button>
                  </div>
                  <FormMessage />
                </FormItem>
              )}
            />
          </form>
        </Form>
      </HoverCardContent>
    </HoverCard>
  );
};

export default NameTooltip;
