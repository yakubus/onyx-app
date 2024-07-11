import { useMemo } from "react";
import { createLazyFileRoute } from "@tanstack/react-router";
import { useSuspenseQueries } from "@tanstack/react-query";

import AccountCard from "@/components/dashboard/accounts/AccountCard";
import TransactionsTable from "@/components/dashboard/accounts/TransactionsTable";

import { getTransactionsQueryOptions } from "@/lib/api/transaction";
import { getAccountsQueryOptions } from "@/lib/api/account";
import { getCategoriesQueryOptions } from "@/lib/api/category";
import { useAccountCardTransactionsData } from "@/lib/hooks/useAccountsCardTransactionsData";

export const Route = createLazyFileRoute(
  "/_dashboard-layout/budget/$budgetId/accounts/$accountId",
)({
  component: Account,
});

function Account() {
  const { accountId, budgetId } = Route.useParams();
  const [{ data: transactions }, { data: accounts }, { data: categories }] =
    useSuspenseQueries({
      queries: [
        getTransactionsQueryOptions(budgetId, accountId, {
          accountId,
        }),
        getAccountsQueryOptions(budgetId),
        getCategoriesQueryOptions(budgetId),
      ],
    });

  const selectedAccount = useMemo(
    () => accounts.find((acc) => acc.id === accountId),
    [accountId, accounts],
  );

  if (!selectedAccount) throw new Error("Incorrect account ID");

  const accountCardTransactionsData =
    useAccountCardTransactionsData(transactions);

  return (
    <div className="px-4 xl:px-8">
      <AccountCard
        selectedAccount={selectedAccount}
        accounts={accounts}
        budgetId={budgetId}
        transactionsData={accountCardTransactionsData}
      />
      <TransactionsTable
        selectedAccount={selectedAccount}
        transactions={transactions}
        categories={categories}
      />
    </div>
  );
}
