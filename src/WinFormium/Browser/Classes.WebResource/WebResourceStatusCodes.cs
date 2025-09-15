// This file is part of the WinFormium project.
// Copyright (c) 2025 Xuanchen Lin all rights reserved.
// This project is licensed under the LGPL License.
// See the LICENSE file in the project root for more information.

namespace WinFormium.Browser;

/// <summary>
/// Provides constants for common HTTP status codes.
/// </summary>
public static class WebResourceStatusCodes
{
    /// <summary>
    /// HTTP status code 100. Indicates that the initial part of a request has been received and has not yet been rejected by the server.
    /// </summary>
    public const int Status100Continue = 100;

    /// <summary>
    /// HTTP status code 101. Indicates that the server understands and is willing to comply with the client's request for a protocol change.
    /// </summary>
    public const int Status101SwitchingProtocols = 101;

    /// <summary>
    /// HTTP status code 102. Indicates that the server has received and is processing the request, but no response is available yet.
    /// </summary>
    public const int Status102Processing = 102;

    /// <summary>
    /// HTTP status code 200. Indicates that the request has succeeded.
    /// </summary>
    public const int Status200OK = 200;

    /// <summary>
    /// HTTP status code 201. Indicates that the request has been fulfilled and has resulted in one or more new resources being created.
    /// </summary>
    public const int Status201Created = 201;

    /// <summary>
    /// HTTP status code 202. Indicates that the request has been accepted for processing, but the processing has not been completed.
    /// </summary>
    public const int Status202Accepted = 202;

    /// <summary>
    /// HTTP status code 203. Indicates that the returned meta-information is not the definitive set as available from the origin server.
    /// </summary>
    public const int Status203NonAuthoritative = 203;

    /// <summary>
    /// HTTP status code 204. Indicates that the server has successfully fulfilled the request and there is no additional content to send.
    /// </summary>
    public const int Status204NoContent = 204;

    /// <summary>
    /// HTTP status code 205. Indicates that the server has fulfilled the request and the user agent should reset the document view.
    /// </summary>
    public const int Status205ResetContent = 205;

    /// <summary>
    /// HTTP status code 206. Indicates that the server is delivering only part of the resource due to a range header sent by the client.
    /// </summary>
    public const int Status206PartialContent = 206;

    /// <summary>
    /// HTTP status code 207. Indicates that the response provides status for multiple independent operations.
    /// </summary>
    public const int Status207MultiStatus = 207;

    /// <summary>
    /// HTTP status code 208. Indicates that members of a DAV binding have already been enumerated in a previous reply to this request.
    /// </summary>
    public const int Status208AlreadyReported = 208;

    /// <summary>
    /// HTTP status code 226. Indicates that the server has fulfilled a GET request for the resource, and the response is a representation of the result of one or more instance-manipulations applied to the current instance.
    /// </summary>
    public const int Status226IMUsed = 226;

    /// <summary>
    /// HTTP status code 300. Indicates multiple options for the resource from which the client may choose.
    /// </summary>
    public const int Status300MultipleChoices = 300;

    /// <summary>
    /// HTTP status code 301. Indicates that the requested resource has been assigned a new permanent URI.
    /// </summary>
    public const int Status301MovedPermanently = 301;

    /// <summary>
    /// HTTP status code 302. Indicates that the requested resource resides temporarily under a different URI.
    /// </summary>
    public const int Status302Found = 302;

    /// <summary>
    /// HTTP status code 303. Indicates that the response to the request can be found under a different URI and should be retrieved using a GET method.
    /// </summary>
    public const int Status303SeeOther = 303;

    /// <summary>
    /// HTTP status code 304. Indicates that the resource has not been modified since the version specified by the request headers.
    /// </summary>
    public const int Status304NotModified = 304;

    /// <summary>
    /// HTTP status code 305. Indicates that the requested resource must be accessed through the proxy given by the Location field.
    /// </summary>
    public const int Status305UseProxy = 305;

    /// <summary>
    /// HTTP status code 306. This code was used in a previous version of the HTTP specification, is no longer used, and is reserved.
    /// </summary>
    public const int Status306SwitchProxy = 306; // RFC 2616, removed

    /// <summary>
    /// HTTP status code 307. Indicates that the requested resource resides temporarily under a different URI and the client should repeat the request with another URI.
    /// </summary>
    public const int Status307TemporaryRedirect = 307;

    /// <summary>
    /// HTTP status code 308. Indicates that the requested resource is now permanently located at another URI.
    /// </summary>
    public const int Status308PermanentRedirect = 308;

    /// <summary>
    /// HTTP status code 400. Indicates that the server cannot or will not process the request due to a client error.
    /// </summary>
    public const int Status400BadRequest = 400;

    /// <summary>
    /// HTTP status code 401. Indicates that the request requires user authentication.
    /// </summary>
    public const int Status401Unauthorized = 401;

    /// <summary>
    /// HTTP status code 402. Reserved for future use; indicates that payment is required.
    /// </summary>
    public const int Status402PaymentRequired = 402;

    /// <summary>
    /// HTTP status code 403. Indicates that the server understood the request but refuses to authorize it.
    /// </summary>
    public const int Status403Forbidden = 403;

    /// <summary>
    /// HTTP status code 404. Indicates that the server has not found anything matching the Request-URI.
    /// </summary>
    public const int Status404NotFound = 404;

    /// <summary>
    /// HTTP status code 405. Indicates that the method specified in the request is not allowed for the resource identified by the request URI.
    /// </summary>
    public const int Status405MethodNotAllowed = 405;

    /// <summary>
    /// HTTP status code 406. Indicates that the resource is only capable of generating response entities which have content characteristics not acceptable according to the accept headers sent in the request.
    /// </summary>
    public const int Status406NotAcceptable = 406;

    /// <summary>
    /// HTTP status code 407. Indicates that the client must first authenticate itself with the proxy.
    /// </summary>
    public const int Status407ProxyAuthenticationRequired = 407;

    /// <summary>
    /// HTTP status code 408. Indicates that the client did not produce a request within the time that the server was prepared to wait.
    /// </summary>
    public const int Status408RequestTimeout = 408;

    /// <summary>
    /// HTTP status code 409. Indicates that the request could not be completed due to a conflict with the current state of the resource.
    /// </summary>
    public const int Status409Conflict = 409;

    /// <summary>
    /// HTTP status code 410. Indicates that the requested resource is no longer available at the server and no forwarding address is known.
    /// </summary>
    public const int Status410Gone = 410;

    /// <summary>
    /// HTTP status code 411. Indicates that the server refuses to accept the request without a defined Content-Length.
    /// </summary>
    public const int Status411LengthRequired = 411;

    /// <summary>
    /// HTTP status code 412. Indicates that one or more conditions given in the request header fields evaluated to false when tested on the server.
    /// </summary>
    public const int Status412PreconditionFailed = 412;

    /// <summary>
    /// HTTP status code 413. Indicates that the request entity is larger than the server is willing or able to process. (RFC 2616, renamed)
    /// </summary>
    public const int Status413RequestEntityTooLarge = 413; // RFC 2616, renamed

    /// <summary>
    /// HTTP status code 413. Indicates that the request payload is larger than the server is willing or able to process. (RFC 7231)
    /// </summary>
    public const int Status413PayloadTooLarge = 413; // RFC 7231

    /// <summary>
    /// HTTP status code 414. Indicates that the URI provided was too long for the server to process. (RFC 2616, renamed)
    /// </summary>
    public const int Status414RequestUriTooLong = 414; // RFC 2616, renamed

    /// <summary>
    /// HTTP status code 414. Indicates that the URI provided was too long for the server to process. (RFC 7231)
    /// </summary>
    public const int Status414UriTooLong = 414; // RFC 7231

    /// <summary>
    /// HTTP status code 415. Indicates that the server is refusing to service the request because the payload is in a format not supported by the requested resource.
    /// </summary>
    public const int Status415UnsupportedMediaType = 415;

    /// <summary>
    /// HTTP status code 416. Indicates that none of the ranges in the request's Range header field overlap the current extent of the selected resource. (RFC 2616, renamed)
    /// </summary>
    public const int Status416RequestedRangeNotSatisfiable = 416; // RFC 2616, renamed

    /// <summary>
    /// HTTP status code 416. Indicates that the range specified by the Range header field in the request can't be fulfilled. (RFC 7233)
    /// </summary>
    public const int Status416RangeNotSatisfiable = 416; // RFC 7233

    /// <summary>
    /// HTTP status code 417. Indicates that the expectation given in the request's Expect header field could not be met by at least one of the inbound servers.
    /// </summary>
    public const int Status417ExpectationFailed = 417;

    /// <summary>
    /// HTTP status code 418. Indicates that the server refuses the attempt to brew coffee with a teapot.
    /// </summary>
    public const int Status418ImATeapot = 418;

    /// <summary>
    /// HTTP status code 419. Indicates that the session has expired or the authentication timeout has occurred. Not defined in any RFC.
    /// </summary>
    public const int Status419AuthenticationTimeout = 419; // Not defined in any RFC

    /// <summary>
    /// HTTP status code 421. Indicates that the request was directed at a server that is not able to produce a response.
    /// </summary>
    public const int Status421MisdirectedRequest = 421;

    /// <summary>
    /// HTTP status code 422. Indicates that the server understands the content type of the request entity, and the syntax of the request entity is correct, but it was unable to process the contained instructions.
    /// </summary>
    public const int Status422UnprocessableEntity = 422;

    /// <summary>
    /// HTTP status code 423. Indicates that the source or destination resource of a method is locked.
    /// </summary>
    public const int Status423Locked = 423;

    /// <summary>
    /// HTTP status code 424. Indicates that the method could not be performed on the resource because the requested action depended on another action and that action failed.
    /// </summary>
    public const int Status424FailedDependency = 424;

    /// <summary>
    /// HTTP status code 426. Indicates that the client should switch to a different protocol.
    /// </summary>
    public const int Status426UpgradeRequired = 426;

    /// <summary>
    /// HTTP status code 428. Indicates that the origin server requires the request to be conditional.
    /// </summary>
    public const int Status428PreconditionRequired = 428;

    /// <summary>
    /// HTTP status code 429. Indicates that the user has sent too many requests in a given amount of time ("rate limiting").
    /// </summary>
    public const int Status429TooManyRequests = 429;

    /// <summary>
    /// HTTP status code 431. Indicates that the server is unwilling to process the request because its header fields are too large.
    /// </summary>
    public const int Status431RequestHeaderFieldsTooLarge = 431;

    /// <summary>
    /// HTTP status code 451. Indicates that the server is denying access to the resource as a consequence of a legal demand.
    /// </summary>
    public const int Status451UnavailableForLegalReasons = 451;

    /// <summary>
    /// HTTP status code 499. This is an unofficial status code originally defined by Nginx and is commonly used in logs when the client has disconnected.
    /// </summary>
    public const int Status499ClientClosedRequest = 499;

    /// <summary>
    /// HTTP status code 500. Indicates that the server encountered an unexpected condition that prevented it from fulfilling the request.
    /// </summary>
    public const int Status500InternalServerError = 500;

    /// <summary>
    /// HTTP status code 501. Indicates that the server does not support the functionality required to fulfill the request.
    /// </summary>
    public const int Status501NotImplemented = 501;

    /// <summary>
    /// HTTP status code 502. Indicates that the server, while acting as a gateway or proxy, received an invalid response from the upstream server.
    /// </summary>
    public const int Status502BadGateway = 502;

    /// <summary>
    /// HTTP status code 503. Indicates that the server is currently unable to handle the request due to a temporary overload or scheduled maintenance.
    /// </summary>
    public const int Status503ServiceUnavailable = 503;

    /// <summary>
    /// HTTP status code 504. Indicates that the server, while acting as a gateway or proxy, did not receive a timely response from the upstream server.
    /// </summary>
    public const int Status504GatewayTimeout = 504;

    /// <summary>
    /// HTTP status code 505. Indicates that the server does not support the HTTP protocol version that was used in the request.
    /// </summary>
    public const int Status505HttpVersionNotsupported = 505;

    /// <summary>
    /// HTTP status code 506. Indicates that the server has an internal configuration error: the chosen variant resource is configured to engage in transparent content negotiation itself, and is therefore not a proper end point in the negotiation process.
    /// </summary>
    public const int Status506VariantAlsoNegotiates = 506;

    /// <summary>
    /// HTTP status code 507. Indicates that the server is unable to store the representation needed to complete the request.
    /// </summary>
    public const int Status507InsufficientStorage = 507;

    /// <summary>
    /// HTTP status code 508. Indicates that the server detected an infinite loop while processing a request.
    /// </summary>
    public const int Status508LoopDetected = 508;

    /// <summary>
    /// HTTP status code 510. Indicates that further extensions to the request are required for the server to fulfill it.
    /// </summary>
    public const int Status510NotExtended = 510;

    /// <summary>
    /// HTTP status code 511. Indicates that the client needs to authenticate to gain network access.
    /// </summary>
    public const int Status511NetworkAuthenticationRequired = 511;
}
