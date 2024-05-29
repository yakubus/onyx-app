import 'package:flutter/material.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:go_router/go_router.dart';
import 'package:onyx_app/views/accounts/widgets/acoount_carousel.dart';

class AccountView extends StatelessWidget {
  const AccountView({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(AppLocalizations.of(context)!.account),
      ),
      body: Column(
        children: [
          const AccountCarousel(),
          IconButton(
              onPressed: () {
                context.go('/add_account');
              },
              icon: const Icon(Icons.add_alarm_rounded))
        ],
      ),
    );
  }
}
