import { FC } from "react";
import { FieldValues, SubmitHandler, useForm } from "react-hook-form";

import { Plus } from "lucide-react";
import { Button } from "../ui/button";
import { Form, FormField, FormItem, FormMessage } from "../ui/form";

interface Props {
  categoriesCount: number;
}

const AddCategoryButton: FC<Props> = ({ categoriesCount }) => {
  const form = useForm({
    defaultValues: {
      name: "",
    },
  });
  const { handleSubmit, control } = form;

  const onSubmit: SubmitHandler<FieldValues> = (data) => {
    console.log(data);
  };

  return (
    <Form {...form}>
      <form onSubmit={handleSubmit(onSubmit)} className="flex items-center">
        <FormField
          control={control}
          disabled={categoriesCount >= 10}
          name="name"
          render={({ field }) => (
            <FormItem className="w-full">
              <input
                placeholder="Add category..."
                {...field}
                className="h-11 w-full rounded-l-md border-y border-l px-8 outline-none"
              />
              <FormMessage />
            </FormItem>
          )}
        />
        <Button
          type="submit"
          variant="secondary"
          className="h-11 rounded-l-none"
          disabled={categoriesCount >= 10}
        >
          <Plus />
        </Button>
      </form>
    </Form>
  );
};

export default AddCategoryButton;
