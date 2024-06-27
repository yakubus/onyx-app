import { queryOptions } from "@tanstack/react-query";
import { privateApi } from "@/lib/axios";
import { getErrorMessage } from "@/lib/utils";
import { AccountResultSchema } from "@/lib/validation/account";
import { AccountType, Money } from "@/lib/validation/base";

export interface CreateAccountPayload {
  name: string;
  balance: Money;
  accountType: AccountType;
}

export interface CreateAccount {
  budgetId: string;
  payload: CreateAccountPayload;
}

interface EditBase {
  budgetId: string;
  accountId: string;
}
export interface EditBalance extends EditBase {
  newBalance: Money;
}

interface EditAccountName extends EditBase {
  newName: string;
}

export const getAccounts = async (budgetId: string) => {
  try {
    const { data } = await privateApi.get(`/${budgetId}/accounts`);
    const validatedData = AccountResultSchema.safeParse(data);
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

export const getAccountsQueryOptions = (budgetId: string) =>
  queryOptions({
    queryKey: ["accounts"],
    queryFn: () => getAccounts(budgetId),
  });

export const createAccount = ({ budgetId, payload }: CreateAccount) =>
  privateApi.post(`/${budgetId}/accounts`, payload);

export const editBalance = ({ budgetId, newBalance, accountId }: EditBalance) =>
  privateApi.put(`/${budgetId}/accounts/${accountId}`, { newBalance });

export const editAccountName = ({
  budgetId,
  newName,
  accountId,
}: EditAccountName) =>
  privateApi.put(`/${budgetId}/accounts/${accountId}`, { newName });
