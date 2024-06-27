import { useSuspenseQueries } from "@tanstack/react-query";
import { createLazyFileRoute } from "@tanstack/react-router";

import { getAccountsQueryOptions } from "@/lib/api/account";
import AccountsCarousel from "@/components/dashboard/accounts/AccountsCarousel";

export const Route = createLazyFileRoute(
  "/_dashboard-layout/budget/$budgetId/accounts",
)({
  component: Accounts,
});

function Accounts() {
  const { budgetId } = Route.useParams();
  const [{ data: accounts }] = useSuspenseQueries({
    queries: [getAccountsQueryOptions(budgetId)],
  });

  return (
    <div>
      <AccountsCarousel accounts={accounts} />
    </div>
  );
}
