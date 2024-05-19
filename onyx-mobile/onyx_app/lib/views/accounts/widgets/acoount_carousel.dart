import 'package:flutter/material.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:carousel_slider_plus/carousel_slider_plus.dart';
import 'package:onyx_app/views/accounts/account_repo.dart';
import 'package:onyx_app/views/accounts/accounts.dart';

class AccountsCarousel extends HookConsumerWidget {
  const AccountsCarousel({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final accountRepo = ref.watch(accountRepoProvider);

    return FutureBuilder<List<Account>>(
      future: accountRepo.getAccounts(),
      builder: (context, snapshot) {
        if (snapshot.connectionState == ConnectionState.waiting) {
          return const CircularProgressIndicator();
        } else if (snapshot.hasError) {
          return Text('Error: ${snapshot.error}');
        } else {
          return CarouselSlider.builder(
            itemCount: snapshot.data!.length,
            itemBuilder: (context, index, realIdx) {
              final account = snapshot.data![index];
              return ListTile(
                title: Text(account.name),
                subtitle: Text(
                    '${account.balance.amount} ${account.balance.currency}'),
              );
            },
            options: CarouselOptions(
              autoPlay: true,
              enlargeCenterPage: true,
            ),
          );
        }
      },
    );
  }
}
