import { FC } from "react";

import { Account } from "@/lib/validation/account";
import {
  Carousel,
  CarouselContent,
  CarouselItem,
  CarouselNext,
  CarouselPrevious,
} from "@/components/ui/carousel";
import CreateAccountForm from "./CreateAccountForm";

interface AccountsCarouselProps {
  accounts: Account[];
}

const AccountsCarousel: FC<AccountsCarouselProps> = ({ accounts }) => {
  return (
    <Carousel>
      <CarouselContent>
        {accounts.length > 0 &&
          accounts.map((account) => (
            <CarouselItem key={account.id}>
              <div className="rounded-xl bg-gradient-to-b from-background via-secondary to-background px-4 py-8 md:px-10 xl:px-16">
                {account.name}
              </div>
            </CarouselItem>
          ))}
        <CarouselItem>
          <div className="rounded-xl bg-gradient-to-b from-background via-secondary to-background px-4 py-8 md:px-10 xl:px-16">
            <CreateAccountForm />
          </div>
        </CarouselItem>
      </CarouselContent>
      <CarouselPrevious className="left-4 hidden bg-primary-foreground md:inline-flex lg:left-8" />
      <CarouselNext className="right-4 hidden bg-primary-foreground md:inline-flex lg:right-8" />
    </Carousel>
  );
};

export default AccountsCarousel;
