import { FC } from "react";

import CarouselCardBalanceForm from "@/components/dashboard/accounts/CarouselCardBalanceForm";
import CarouselCardNameForm from "@/components/dashboard/accounts/CarouselCardNameForm";
import CarouselCardDatePicker from "@/components/dashboard/accounts/CarouselCardDatePicker";
import DeleteAccountButton from "@/components/dashboard/accounts/DeleteAccountButton";

import { Account } from "@/lib/validation/account";
import { cn, formatAmount } from "@/lib/utils";

interface AccountCarouselCardProps {
  account: Account;
}

const AccountCarouselCard: FC<AccountCarouselCardProps> = ({ account }) => {
  return (
    <div
      className={cn(
        "grid h-full w-full grid-cols-1 place-items-center gap-y-10 md:grid-cols-5 md:gap-x-2 md:gap-y-0 xl:grid-cols-2",
        account.optimistic && "opacity-50",
      )}
    >
      <div className="w-full max-w-[400px] space-y-2 justify-self-center rounded-xl bg-gradient-to-b from-primary via-primary to-primaryDark p-4 text-primary-foreground shadow-lg shadow-primaryDark/50 md:col-span-3 md:w-4/5 xl:col-span-1 xl:w-3/4 xl:max-w-full">
        <div className="flex items-center space-x-2 text-lg text-primary-foreground md:text-2xl">
          <div className="flex-1">
            <CarouselCardNameForm
              defaultName={account.name}
              accountId={account.id}
            />
          </div>
          <DeleteAccountButton accountId={account.id} />
        </div>
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
      <div className="col-span-2 m-auto w-full max-w-[400px] space-y-10 place-self-center md:w-full xl:col-span-1">
        <CarouselCardDatePicker />
        <div className="grid h-fit grid-cols-2 md:w-full">
          <div className="mr-2 space-y-2 px-1">
            <h3 className="text-xl font-semibold">Income:</h3>
            <p className="text-lg font-semibold">
              PLN {formatAmount("123456789.67")}
            </p>
          </div>
          <div className="ml-2 space-y-2 px-1">
            <h3 className="text-xl font-semibold">Outcome:</h3>
            <p className="text-lg font-semibold text-destructive">
              PLN {formatAmount("123456789.67")}
            </p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AccountCarouselCard;
