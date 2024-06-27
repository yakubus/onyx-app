import { FC } from "react";

import CarouselCardBalanceForm from "@/components/dashboard/accounts/CarouselCardBalanceForm";
import CarouselCardNameForm from "@/components/dashboard/accounts/CarouselCardNameForm";

import { Account } from "@/lib/validation/account";

interface AccountCarouselCardProps {
  account: Account;
}

const AccountCarouselCard: FC<AccountCarouselCardProps> = ({ account }) => {
  return (
    <div className="h-full md:grid md:grid-cols-3 md:place-items-center md:gap-x-6">
      <div className="w-full space-y-2 rounded-xl bg-gradient-to-b from-primary via-primary to-primaryDark p-4 text-primary-foreground shadow-lg shadow-primaryDark/50 md:col-span-2 md:w-3/5 xl:w-1/2">
        <h3 className="text-lg text-primary-foreground md:text-2xl">
          <CarouselCardNameForm
            defaultName={account.name}
            accountId={account.id}
          />
        </h3>
        <div className="pl-3">
          <span className="text-xs font-thin">BALANCE</span>
          <CarouselCardBalanceForm
            balance={account.balance}
            accountId={account.id}
          />
        </div>
        <div className="flex w-full justify-end space-x-4 pr-3">
          <span className="text-sm">Account type:</span>
          <span className="text-sm tracking-wide">
            {account.type.toUpperCase()}
          </span>
        </div>
      </div>
    </div>
  );
};

export default AccountCarouselCard;
