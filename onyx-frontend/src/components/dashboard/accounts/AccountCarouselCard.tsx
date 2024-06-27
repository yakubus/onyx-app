import { FC } from "react";

import CarouselCardBalanceForm from "./CarouselCardBalanceForm";

import { Account } from "@/lib/validation/account";

interface AccountCarouselCardProps {
  account: Account;
}

const AccountCarouselCard: FC<AccountCarouselCardProps> = ({ account }) => {
  return (
    <div className="h-full md:grid md:grid-cols-3 md:place-items-center md:gap-x-6">
      <div className="w-full space-y-2 rounded-xl bg-gradient-to-b from-primary via-primary to-primaryDark p-4 text-primary-foreground shadow-lg shadow-primaryDark/50 md:col-span-2 md:w-3/5 xl:w-1/2">
        <h3 className="text-lg text-primary-foreground md:text-2xl">
          {account.name}
        </h3>
        <div>
          <span className="text-xs font-thin">BALANCE</span>
          <CarouselCardBalanceForm
            balance={account.balance}
            accountId={account.id}
          />
        </div>
        <div className="flex w-full justify-end space-x-4">
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
