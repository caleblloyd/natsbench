# natsbench
## core
https://docs.nats.io/reference/reference-protocols/nats-protocol
```
S: INFO {"option_name":option_value,...}␍␊
C: CONNECT {"option_name":option_value,...}␍␊
C: PUB <subject> [reply-to] <#bytes>␍␊[payload]␍␊
C: HPUB <subject> [reply-to] <#header bytes> <#total-bytes>␍␊[headers]␍␊␍␊[payload]␍␊
C: SUB <subject> [queue group] <sid>␍␊
C: UNSUB <sid> [max_msgs]␍␊
S: MSG <subject> <sid> [reply-to] <#bytes>␍␊[payload]␍␊
S: HMSG <subject> <sid> [reply-to] <#header-bytes> <#total-bytes>␍␊[headers]␍␊␍␊[payload]␍␊
B: PING␍␊
B: PONG␍␊
S: +OK␍␊
S: -ERR <error message>␍␊
```

## JetStream

### Manage
```
SUB: $JS.API.STREAM.CREATE.{stream}
REQ: jetstream/api/v1/stream_create_request
RES: jetstream/api/v1/stream_create_response

SUB: LIST
SUB: GET
SUB: DELETE
```

### Pull Next
```
C: SUB _INBOX.<id> <sid>␍␊
C: PUB $JS.API.CONSUMER.MSG.NEXT.<stream>.<consumer> _INBOX.<id> 0␍␊
S: MSG <stream-subject> <sid> $JS.ACK.<stream>.<consumer>.3.1.3.1692271055168144700.0 <#bytes>␍␊[payload]␍␊
```
