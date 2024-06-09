// ignore_for_file: public_member_api_docs, sort_constructors_first
import 'package:flutter/material.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:onyx_app/config.dart';

class ScafoldOnyx extends HookConsumerWidget {
  final PreferredSizeWidget? appBar;
  final Widget body;

  const ScafoldOnyx({
    super.key,
    this.appBar,
    required this.body,
  });

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    return Scaffold(
        appBar: appBar,
        body: Padding(
          padding: const EdgeInsets.all(Config.PADDING),
          child: body,
        ));
  }
}
