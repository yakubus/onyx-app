import { FC, useMemo } from "react";

import AccountCardBalanceForm from "@/components/dashboard/accounts/AccountCardBalanceForm";
import AccountCardNameForm from "@/components/dashboard/accounts/AccountCardNameForm";
import AccountCardDatePicker from "@/components/dashboard/accounts/AccountCardDatePicker";
import AccountCardDeleteButton from "@/components/dashboard/accounts/AccountCardDeleteButton";

import { Account } from "@/lib/validation/account";
import { cn, getFormattedCurrency } from "@/lib/utils";
import { Button } from "@/components/ui/button";
import { ArrowLeft, ArrowRight } from "lucide-react";
import { Link, useSearch } from "@tanstack/react-router";

interface TransactionsData {
  [x: number]: {
    balance: {
      [x: number]: {
        income: number;
        expenses: number;
      };
    };
  };
  availableDates: {
    [x: number]: number[];
  };
}

interface AccountCardProps {
  selectedAccount: Account;
  accounts: Account[];
  budgetId: string;
  transactionsData: TransactionsData;
}

const AccountCard: FC<AccountCardProps> = ({
  selectedAccount,
  accounts,
  budgetId,
  transactionsData,
}) => {
  const { accMonth, accYear } = useSearch({
    from: "/_dashboard-layout/budget/$budgetId/accounts/$accountId",
  });
  const selectedAccountIndex = useMemo(
    () => accounts.findIndex((a) => a.id === selectedAccount.id),
    [accounts, selectedAccount.id],
  );
  const nextAccountId =
    selectedAccountIndex < accounts.length - 1 &&
    accounts[selectedAccountIndex + 1].id;
  const prevAccountId =
    selectedAccountIndex !== 0 && accounts[selectedAccountIndex - 1].id;

  return (
    <div
      className={cn(
        "relative grid grid-cols-1 gap-y-10 rounded-xl border bg-card px-4 py-6 shadow-md md:grid-cols-2 md:gap-x-10 md:gap-y-0 lg:gap-x-20",
        selectedAccount.optimistic && "opacity-50",
      )}
    >
      {nextAccountId && (
        <Button
          variant="outline"
          size="icon"
          className="absolute right-5 top-1/2 hidden -translate-y-1/2 rounded-full  lg:inline-flex"
          asChild
        >
          <Link
            to={`/budget/${budgetId}/accounts/${nextAccountId}`}
            search={(prev) => prev}
            mask={{
              to: `/budget/${budgetId}/accounts/${nextAccountId}`,
            }}
          >
            <ArrowRight />
          </Link>
        </Button>
      )}
      {prevAccountId && (
        <Button
          variant="outline"
          size="icon"
          className="absolute left-5 top-1/2 hidden -translate-y-1/2 rounded-full disabled:opacity-50 lg:inline-flex"
          asChild
        >
          <Link
            to={`/budget/${budgetId}/accounts/${prevAccountId}`}
            search={(prev) => prev}
            mask={{
              to: `/budget/${budgetId}/accounts/${prevAccountId}`,
            }}
          >
            <ArrowLeft />
          </Link>
        </Button>
      )}

      <div className="w-full max-w-[400px] space-y-2 justify-self-center rounded-xl bg-gradient-to-b from-primary via-primary to-primaryDark p-4 text-primary-foreground shadow-lg shadow-primaryDark/50 lg:justify-self-end">
        <div className="flex items-center space-x-2 text-lg text-primary-foreground md:text-2xl">
          <div className="flex-1">
            <AccountCardNameForm
              defaultName={selectedAccount.name}
              accountId={selectedAccount.id}
            />
          </div>
          <AccountCardDeleteButton accountId={selectedAccount.id} />
        </div>
        <div className="pl-3">
          <span className="text-xs font-thin">BALANCE</span>
          <div className="-ml-1">
            <AccountCardBalanceForm
              balance={selectedAccount.balance}
              accountId={selectedAccount.id}
            />
          </div>
        </div>
        <div className="flex w-full justify-end space-x-4 pr-3">
          <span className="text-sm">Account type:</span>
          <span className="text-sm tracking-wide">
            {selectedAccount.type.toUpperCase()}
          </span>
        </div>
      </div>
      <div className="m-auto max-w-[400px] space-y-10 md:mx-0 md:my-auto md:w-auto md:space-y-4 lg:justify-self-start">
        <AccountCardDatePicker
          availableDates={transactionsData.availableDates}
        />
        <div className="grid h-fit grid-cols-2 md:w-full">
          <div className="mr-2 space-y-2 px-1">
            <h3 className="text-xl font-semibold">Income:</h3>
            <p className="space-x-2 text-lg font-semibold">
              {getFormattedCurrency(
                transactionsData[Number(accYear)].balance[Number(accMonth)]
                  .income || 0,
                selectedAccount.balance.currency,
              )}
            </p>
          </div>
          <div className="ml-2 space-y-2 px-1">
            <h3 className="text-xl font-semibold">Expenses:</h3>
            <p className="space-x-2 text-lg font-semibold">
              {getFormattedCurrency(
                transactionsData[Number(accYear)].balance[Number(accMonth)]
                  .expenses || 0,
                selectedAccount.balance.currency,
              )}
            </p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AccountCard;
