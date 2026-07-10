import 'package:flutter/material.dart';
import 'package:flutter_sms_inbox/flutter_sms_inbox.dart';
import 'package:permission_handler/permission_handler.dart';

void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'SMS Reader',
      theme: ThemeData(primarySwatch: Colors.blue, useMaterial3: true),
      home: const SmsReaderScreen(),
    );
  }
}

class SmsReaderScreen extends StatefulWidget {
  const SmsReaderScreen({super.key});

  @override
  State<SmsReaderScreen> createState() => _SmsReaderScreenState();
}

class _SmsReaderScreenState extends State<SmsReaderScreen> {
  final SmsQuery _query = SmsQuery();
  List<SmsMessage> _messages = [];

  @override
  void initState() {
    super.initState();
    _fetchSms();
  }

  Future<void> _fetchSms() async {
    var status = await Permission.sms.status;
    if (status.isDenied) {
      status = await Permission.sms.request();
    }

    if (status.isGranted) {
      final messages = await _query.querySms(kinds: [SmsQueryKind.inbox]);
      setState(() {
        _messages = messages;
      });
    } else {
      debugPrint('SMS permission denied');
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('SMS Reader'),
        actions: [
          IconButton(icon: const Icon(Icons.refresh), onPressed: _fetchSms),
        ],
      ),
      body: _messages.isEmpty
          ? const Center(
              child: Text(
                'No messages found or permission denied.\nTap refresh to try again.',
                textAlign: TextAlign.center,
              ),
            )
          : ListView.builder(
              itemCount: _messages.length,
              itemBuilder: (context, index) {
                final message = _messages[index];
                return Card(
                  margin: const EdgeInsets.symmetric(
                    horizontal: 10,
                    vertical: 5,
                  ),
                  child: ListTile(
                    title: Text(
                      message.address ?? 'Unknown Sender',
                      style: const TextStyle(fontWeight: FontWeight.bold),
                    ),
                    subtitle: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        const SizedBox(height: 5),
                        Text(message.body ?? ''),
                        const SizedBox(height: 5),
                        Text(
                          message.date != null
                              ? message.date!.toLocal().toString()
                              : 'Unknown Time',
                          style: TextStyle(
                            fontSize: 12,
                            color: Colors.grey[600],
                          ),
                        ),
                      ],
                    ),
                    isThreeLine: true,
                  ),
                );
              },
            ),
    );
  }
}
