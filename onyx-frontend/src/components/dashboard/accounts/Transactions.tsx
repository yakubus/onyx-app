import { FC, useMemo } from "react";
import { useQuery } from "@tanstack/react-query";
import { useParams, useSearch } from "@tanstack/react-router";

import {
  getTransactions,
  getTransactionsQueryKey,
} from "@/lib/api/transaction";
import { Account } from "@/lib/validation/account";
import TransactionsNavbar from "./TransactionsNavbar";
import TransactionsTable from "./TransactionsTable";

interface Transactions {
  accounts: Account[];
}

const Transactions: FC<Transactions> = ({ accounts }) => {
  const { budgetId } = useParams({
    from: "/_dashboard-layout/budget/$budgetId/accounts",
  });
  const { selectedAcc } = useSearch({
    from: "/_dashboard-layout/budget/$budgetId/accounts",
  });
  const { data, isPending, isError } = useQuery({
    queryKey: getTransactionsQueryKey(selectedAcc || accounts[0].id),
    queryFn: () => getTransactions(budgetId, { accountId: selectedAcc }),
    enabled: !!accounts.length && !!selectedAcc,
  });
  const selectedAccount = useMemo(
    () => accounts.find((a) => a.id === selectedAcc),
    [accounts, selectedAcc],
  );

  console.log(data);

  if (isPending) {
    return <TableLoadingSkeleton />;
  }

  if (isError) {
    return <div>error</div>;
  }

  return (
    <div className="flex h-full flex-col p-10">
      {selectedAccount && <TransactionsNavbar account={selectedAccount} />}
      <div className="flex flex-grow items-center justify-center">
        <TransactionsTable data={data} />
      </div>
    </div>
  );
};

export default Transactions;

const TableLoadingSkeleton = () => (
  <div className="h-full animate-pulse overflow-auto py-8 scrollbar-none md:pl-14 md:pr-10 md:pt-14">
    <div className="overflow-hidden rounded-lg border">
      <ul>
        <li className="h-20 w-full border-t bg-accent/30"></li>
        <li className="h-20 w-full border-t bg-accent/30"></li>
        <li className="h-20 w-full border-t bg-accent/30"></li>
        <li className="h-20 w-full border-t bg-accent/30"></li>
      </ul>
    </div>
  </div>
);
