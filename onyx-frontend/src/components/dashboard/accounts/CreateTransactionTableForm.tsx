import { Button } from "@/components/ui/button";
import { Form, FormField } from "@/components/ui/form";
import { Plus } from "lucide-react";
import { FC } from "react";
import { useForm } from "react-hook-form";

interface CreateTransactionTableFormProps {}

const CreateTransactionTableForm: FC<
  CreateTransactionTableFormProps
> = ({}) => {
  const form = useForm({
    defaultValues: {},
  });
  const { control } = form;
  return (
    <Form {...form}>
      <form className="overflow-hidden">
        <Button size="icon" type="submit">
          <Plus />
        </Button>
      </form>
    </Form>
  );
};

export default CreateTransactionTableForm;
