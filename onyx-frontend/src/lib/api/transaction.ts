import { privateApi } from "@/lib/axios";
import { TransactionResultSchema } from "@/lib/validation/transaction";
import { getErrorMessage } from "@/lib/utils";
import { queryOptions } from "@tanstack/react-query";
import { Money } from "@/lib/validation/base";

interface QueryParams {
  query?: string;
  counterpartyId?: string;
  accountId?: string;
  subcategoryId?: string;
}

export interface CreateTransactionPayload {
  accountId: string;
  amount: Money;
  transactedAt: Date;
  counterpartyName: string;
  subcategoryId: string;
}

interface CreateTransaction {
  budgetId: string;
  payload: CreateTransactionPayload;
}

export const getTransactions = async (
  budgetId: string,
  search: QueryParams,
) => {
  const { accountId, counterpartyId, query, subcategoryId } = search;
  const searchParams = new URLSearchParams({
    ...(accountId && { accountId }),
    ...(counterpartyId && { counterpartyId }),
    ...(query && { query }),
    ...(subcategoryId && { subcategoryId }),
  });

  let url = `/${budgetId}/transactions`;

  if (searchParams.toString()) {
    url += `?${searchParams.toString()}`;
  }

  try {
    const { data } = await privateApi.get(url);
    const validatedData = TransactionResultSchema.safeParse(data);
    if (!validatedData.success) {
      console.log(validatedData.error?.issues);
      throw new Error("Invalid data type.");
    }

    const { value, isFailure, error } = validatedData.data;
    if (isFailure) {
      throw new Error(error.message);
    }

    return value;
  } catch (error) {
    console.error(getErrorMessage(error));
    throw new Error(getErrorMessage(error));
  }
};

export const getTransactionsQueryKey = (accountId: string) => [
  "transactions",
  accountId,
];

export const getTransactionsQueryOptions = (
  budgetId: string,
  accountId: string,
  search: QueryParams,
) =>
  queryOptions({
    queryKey: ["transactions", accountId],
    queryFn: () => getTransactions(budgetId, search),
  });

export const createTransaction = ({ budgetId, payload }: CreateTransaction) =>
  privateApi.post(`/${budgetId}/transactions`, payload);
