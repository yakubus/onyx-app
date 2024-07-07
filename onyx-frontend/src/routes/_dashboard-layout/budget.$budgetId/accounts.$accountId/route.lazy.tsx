import { createLazyFileRoute } from "@tanstack/react-router";

import { useSuspenseQueries } from "@tanstack/react-query";
import { getTransactionsQueryOptions } from "@/lib/api/transaction";
import { getAccountsQueryOptions } from "@/lib/api/account";
import AccountCard from "@/components/dashboard/accounts/AccountCard";
import { useMemo } from "react";

export const Route = createLazyFileRoute(
  "/_dashboard-layout/budget/$budgetId/accounts/$accountId",
)({
  component: Account,
});

function Account() {
  const { accountId, budgetId } = Route.useParams();
  const [{ data: transactions }, { data: accounts }] = useSuspenseQueries({
    queries: [
      getTransactionsQueryOptions(budgetId, accountId, {
        accountId,
      }),
      getAccountsQueryOptions(budgetId),
    ],
  });

  const selectedAccount = useMemo(
    () => accounts.find((acc) => acc.id === accountId),
    [accountId, accounts],
  );

  if (!selectedAccount) throw new Error("Incorrect account ID");

  return (
    <div className="p-4">
      <AccountCard
        selectedAccount={selectedAccount}
        accounts={accounts}
        budgetId={budgetId}
      />
    </div>
  );
}
