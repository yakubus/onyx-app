import axios from "axios";
import { FieldValues, SubmitHandler, useForm } from "react-hook-form";
import { useMutation } from "@tanstack/react-query";
import { useNavigate, useRouter } from "@tanstack/react-router";

import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { Form, FormField, FormItem } from "@/components/ui/form";
import { UserWithTokenResult } from "@/lib/validation/user";
import { useAuth } from "@/lib/hooks/useAuth";
import { sleep } from "@/lib/utils";

const LoginButton = () => {
  const router = useRouter();
  const navigate = useNavigate();
  const auth = useAuth();

  const form = useForm<FieldValues>({
    defaultValues: {
      email: "",
      password: "",
    },
  });
  const { control, handleSubmit } = form;

  const { mutate } = useMutation({
    mutationFn: async ({
      email,
      password,
    }: {
      email: string;
      password: string;
    }) => {
      const response = await axios.post<UserWithTokenResult>(
        "/api/user/login",
        { email, password },
      );
      return response.data;
    },
    onSuccess: async (data) => {
      await auth.login(data.value);
      await sleep(1);
      await router.invalidate();
      await navigate({ to: "/budget" });
    },
  });

  const onSubmit: SubmitHandler<FieldValues> = (data) => {
    mutate({
      email: data.email,
      password: data.password,
    });
  };

  return (
    <Dialog>
      <DialogTrigger asChild>
        <Button
          variant="outline"
          className="mt-0 h-30px w-26 rounded-full text-base font-semibold lg:mx-0"
        >
          Login
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[450px]">
        <DialogHeader className="divide-y-2">
          <DialogTitle className="my-2 text-center text-3xl font-bold">
            Sign In
          </DialogTitle>
          <DialogDescription>
            <p className="mb-4 mt-6 text-foreground">
              Lorem ipsum dolor sit amet consectetur adipisicing elit. Quae
              perferendis labore.
            </p>
          </DialogDescription>
        </DialogHeader>
        <Form {...form}>
          <form className="grid gap-4" onSubmit={handleSubmit(onSubmit)}>
            <FormField
              control={control}
              name="email"
              render={({ field }) => (
                <FormItem>
                  <Input {...field} placeholder="Email..." />
                </FormItem>
              )}
            />
            <FormField
              control={control}
              name="password"
              render={({ field }) => (
                <FormItem>
                  <Input {...field} type="password" placeholder="Password..." />
                </FormItem>
              )}
            />
            <Button
              type="submit"
              className="mx-auto h-14 w-full rounded-full text-base font-semibold"
            >
              Sign in
            </Button>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  );
};
export default LoginButton;
