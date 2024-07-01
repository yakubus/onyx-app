import { useEffect } from "react";
import { useSuspenseQueries } from "@tanstack/react-query";
import { createLazyFileRoute, useNavigate } from "@tanstack/react-router";

import { getAccountsQueryOptions } from "@/lib/api/account";
import AccountsCarousel from "@/components/dashboard/accounts/AccountsCarousel";
import TransactionsTable from "@/components/dashboard/accounts/TransactionsTable";

export const Route = createLazyFileRoute(
  "/_dashboard-layout/budget/$budgetId/accounts",
)({
  component: Accounts,
});

function Accounts() {
  const { budgetId } = Route.useParams();
  const { selectedAcc } = Route.useSearch();
  const navigate = useNavigate();

  const [{ data: accounts }] = useSuspenseQueries({
    queries: [getAccountsQueryOptions(budgetId)],
  });

  useEffect(() => {
    const updateSelectedAcc = async () => {
      if (!selectedAcc && accounts.length > 0) {
        await navigate({
          search: (prev) => ({ ...prev, selectedAcc: accounts[0].id }),
          mask: "/budget/$budgetId/accounts",
        });
      }
    };
    updateSelectedAcc();
  }, []);

  return (
    <div>
      <AccountsCarousel accounts={accounts} />
      {accounts.length > 0 ? (
        <TransactionsTable accounts={accounts} />
      ) : (
        <h2>Create your first account and add transactions.</h2>
      )}
    </div>
  );
}
