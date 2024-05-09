import 'models/money.dart';
import 'views/accounts/accounts.dart';

class Mocks {
  static List<Account> accountsMock = [
    Account(
      id: '1',
      name: 'Account 1',
      balance: Money(amount: 100, currency: 'USD'),
      type: 'Checking',
    ),
    Account(
      id: '2',
      name: 'Account 2',
      balance: Money(amount: 200, currency: 'PLN'),
      type: 'Savings',
    ),
    Account(
      id: '3',
      name: 'Account 3',
      balance: Money(amount: 300, currency: 'PLN'),
      type: 'Checking',
    ),
    Account(
      id: '4',
      name: 'Account 4',
      balance: Money(amount: 400, currency: 'PLN'),
      type: 'Savings',
    ),
    Account(
      id: '5',
      name: 'Account 5',
      balance: Money(amount: 500, currency: 'USD'),
      type: 'Checking',
    ),
    Account(
      id: '6',
      name: 'Account 6',
      balance: Money(amount: 600, currency: 'USD'),
      type: 'Savings',
    ),
    Account(
      id: '7',
      name: 'Account 7',
      balance: Money(amount: 700, currency: 'USD'),
      type: 'Checking',
    ),
    Account(
      id: '8',
      name: 'Account 8',
      balance: Money(amount: 800, currency: 'USD'),
      type: 'Savings',
    ),
    Account(
      id: '9',
      name: 'Account 9',
      balance: Money(amount: 900, currency: 'USD'),
      type: 'Checking',
    ),
    Account(
      id: '10',
      name: 'Account 10',
      balance: Money(amount: 1000, currency: 'USD'),
      type: 'Savings',
    ),
  ];
}
