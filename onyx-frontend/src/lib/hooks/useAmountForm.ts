import { useEffect } from "react";
import { useForm } from "react-hook-form";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useToast } from "@/components/ui/use-toast";

const useAmountForm = ({
  defaultAmount,
  queryKey,
  mutationFn,
}: {
  defaultAmount: string;
  queryKey: string[];
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  mutationFn: (args: any) => Promise<any>;
}) => {
  const queryClient = useQueryClient();
  const { toast } = useToast();

  const form = useForm({
    defaultValues: {
      amount: defaultAmount,
    },
  });

  const {
    handleSubmit,
    control,
    formState: { isDirty },
    reset,
  } = form;

  const { mutate, isError } = useMutation({
    mutationFn,
    onSettled: async () => {
      return await queryClient.invalidateQueries({ queryKey });
    },
    onError: (error) => {
      console.error("Mutation error:", error);
      toast({
        title: "Error",
        description: "Oops... Something went wrong. Please try again later.",
      });
    },
  });

  useEffect(() => {
    reset({ amount: defaultAmount });
  }, [defaultAmount, reset]);

  useEffect(() => {
    if (isError) {
      reset({ amount: defaultAmount });
    }
  }, [isError, reset, defaultAmount]);

  return {
    form,
    handleSubmit,
    control,
    isDirty,
    mutate,
  };
};

export default useAmountForm;
