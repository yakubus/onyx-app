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
import { FieldValues, SubmitHandler, useForm } from "react-hook-form";
import { Form, FormField, FormItem } from "../ui/form";

const LoginButton = () => {
  const form = useForm<FieldValues>({
    defaultValues: {
      email: "",
      password: "",
    },
  });
  const { control, handleSubmit } = form;

  const onSubmit: SubmitHandler<FieldValues> = (data) => {
    console.log(data);
  };

  return (
    <Dialog>
      <DialogTrigger asChild>
        <Button
          variant="outline"
          className="mx-auto mt-16 h-16 w-56 rounded-full text-base font-semibold lg:mx-0"
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
