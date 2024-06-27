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
import AccountCarouselCard from "./AccountCarouselCard";
import { cn } from "@/lib/utils";
import { useCarouselKeyboardDisable } from "@/lib/hooks/useCarouselKeyboardDisable";

interface AccountsCarouselProps {
  accounts: Account[];
}

const AccountsCarousel: FC<AccountsCarouselProps> = ({ accounts }) => {
  const emblaRef = useCarouselKeyboardDisable();

  return (
    <Carousel ref={emblaRef}>
      <CarouselContent className="ml-0">
        {accounts.length > 0 &&
          accounts.map((account) => (
            <CarouselItem
              key={account.id}
              className="border-y pl-0 first-of-type:rounded-l first-of-type:border-l"
            >
              <div className="h-full w-full bg-card px-4 py-7 md:px-8 xl:px-16">
                <AccountCarouselCard account={account} />
              </div>
            </CarouselItem>
          ))}
        <CarouselItem
          className={cn(
            "rounded-r border-r pl-0",
            accounts.length === 0 && "rounded-l border-l",
          )}
        >
          <div className="h-full w-full border-y bg-card px-4 py-7 md:px-8 xl:px-16">
            <CreateAccountForm />
          </div>
        </CarouselItem>
      </CarouselContent>
      <CarouselPrevious className="left-4 hidden md:inline-flex lg:left-8" />
      <CarouselNext className="right-4 hidden md:inline-flex lg:right-8" />
    </Carousel>
  );
};

export default AccountsCarousel;
