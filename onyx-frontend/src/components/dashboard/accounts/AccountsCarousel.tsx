import { FC, useEffect, useState } from "react";
import { useNavigate, useSearch } from "@tanstack/react-router";

import CreateAccountForm from "@/components/dashboard/accounts/CreateAccountForm";
import AccountCarouselCard from "@/components/dashboard/accounts//AccountCarouselCard";
import {
  Carousel,
  CarouselContent,
  CarouselItem,
  CarouselNext,
  CarouselPrevious,
} from "@/components/ui/carousel";

import { cn } from "@/lib/utils";
import { type CarouselApi } from "@/components/ui/carousel";
import { type Account } from "@/lib/validation/account";

interface AccountsCarouselProps {
  accounts: Account[];
}

const AccountsCarousel: FC<AccountsCarouselProps> = ({ accounts }) => {
  const { selectedAcc } = useSearch({
    from: "/_dashboard-layout/budget/$budgetId/accounts",
  });
  const navigate = useNavigate();
  const startIndex = Math.max(
    0,
    accounts.findIndex((acc) => acc.id === selectedAcc),
  );
  const [api, setApi] = useState<CarouselApi | null>(null);

  useEffect(() => {
    if (api) {
      api.scrollTo(startIndex);

      const onSelect = () => {
        const i = api.selectedScrollSnap();
        if (i >= accounts.length) return;
        if (selectedAcc === accounts[i].id) return;
        navigate({
          search: (prev) => ({ ...prev, selectedAcc: accounts[i]?.id }),
          mask: "/budget/$budgetId/accounts",
        });
      };

      api.on("select", onSelect);

      return () => {
        api.off("select", onSelect);
      };
    }
  }, [api, startIndex, accounts, navigate]);

  return (
    <Carousel setApi={setApi}>
      <CarouselContent className="ml-0">
        {accounts.length > 0 &&
          accounts.map((account, i) => (
            <CarouselItem
              key={account.id}
              tabIndex={i}
              className="border-y pl-0 first-of-type:rounded-l first-of-type:border-l"
            >
              <div className="h-full w-full bg-card px-4 py-7 md:px-14 xl:px-20">
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
          <div className="h-full w-full border-y bg-card px-4 py-7 md:px-14 xl:px-20">
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
