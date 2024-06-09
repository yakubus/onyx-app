import 'package:flutter/widgets.dart';
import 'package:onyx_app/themes/scafold_onyx.dart';
import 'package:onyx_app/widgets/appbar.dart';
import 'package:flutter/material.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';

class Home extends HookConsumerWidget {
  const Home({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    return const ScafoldOnyx(
      appBar: DefaultAppBar(
        title: "ONYX",
      ),
      body: Column(
        children: [],
      ),
    );
  }
}
