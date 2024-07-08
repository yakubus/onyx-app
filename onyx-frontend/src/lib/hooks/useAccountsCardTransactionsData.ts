import { useMemo } from "react";
import { Transaction } from "@/lib/validation/transaction";
import {
  DEFAULT_MONTH_NUMBER,
  DEFAULT_YEAR_NUMBER,
} from "@/lib/constants/date";

export const useAccountCardTransactionsData = (transactions: Transaction[]) => {
  return useMemo(() => {
    return transactions.reduce(
      (acc, transaction) => {
        const currentTransactionDate = new Date(transaction.transactedAt);
        const currentTransactionYear = currentTransactionDate.getFullYear();
        const currentTransactionMonth = currentTransactionDate.getMonth() + 1;
        const isExpense = transaction.amount.amount < 0;

        if (!acc[currentTransactionYear]) {
          acc[currentTransactionYear] = {
            balance: {},
          };
        }

        if (!acc[currentTransactionYear].balance[currentTransactionMonth]) {
          acc[currentTransactionYear].balance[currentTransactionMonth] = {
            income: 0,
            expenses: 0,
          };
        }

        const currentMonthBalance =
          acc[currentTransactionYear].balance[currentTransactionMonth];

        if (!acc.availableDates[currentTransactionYear]) {
          acc.availableDates[currentTransactionYear] = [];
        }

        const currentAvailableDates =
          acc.availableDates[currentTransactionYear];

        if (isExpense) {
          currentMonthBalance.expenses -= transaction.amount.amount;
        } else {
          currentMonthBalance.income += transaction.amount.amount;
        }

        if (!currentAvailableDates.includes(currentTransactionMonth)) {
          currentAvailableDates.push(currentTransactionMonth);
          currentAvailableDates.sort((a, b) => a - b);
        }

        return acc;
      },
      {
        [DEFAULT_YEAR_NUMBER]: {
          balance: {
            [DEFAULT_MONTH_NUMBER - 1]: {
              income: 0,
              expenses: 0,
            },
            [DEFAULT_MONTH_NUMBER]: {
              income: 0,
              expenses: 0,
            },
          },
        },
        availableDates: {
          [DEFAULT_YEAR_NUMBER]: [
            DEFAULT_MONTH_NUMBER - 1,
            DEFAULT_MONTH_NUMBER,
          ],
        },
      },
    );
  }, [transactions]);
};
