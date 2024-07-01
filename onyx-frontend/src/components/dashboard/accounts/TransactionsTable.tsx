import { FC } from "react";
import { useQuery } from "@tanstack/react-query";
import { useParams, useSearch } from "@tanstack/react-router";

import {
  getTransactions,
  getTransactionsQueryKey,
} from "@/lib/api/transaction";
import { Account } from "@/lib/validation/account";

interface TransactionsTableProps {
  accounts: Account[];
}

const TransactionsTable: FC<TransactionsTableProps> = ({ accounts }) => {
  const { budgetId } = useParams({
    from: "/_dashboard-layout/budget/$budgetId/accounts",
  });
  const { selectedAcc } = useSearch({
    from: "/_dashboard-layout/budget/$budgetId/accounts",
  });
  const { data, isPending } = useQuery({
    queryKey: getTransactionsQueryKey(selectedAcc || accounts[0].id),
    queryFn: () => getTransactions(budgetId, { accountId: selectedAcc }),
    enabled: !!accounts.length && !!selectedAcc,
  });

  if (isPending) {
    return <div>loading...</div>;
  }

  return <div className="p-10">{JSON.stringify(data)}</div>;
};

export default TransactionsTable;
