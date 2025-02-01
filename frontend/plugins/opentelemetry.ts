import { ConsoleSpanExporter, SimpleSpanProcessor, WebTracerProvider } from "@opentelemetry/sdk-trace-web";
import { ZoneContextManager } from '@opentelemetry/context-zone';
import { Resource } from '@opentelemetry/resources';
import { ATTR_SERVICE_NAME } from '@opentelemetry/semantic-conventions';
import { registerInstrumentations } from '@opentelemetry/instrumentation';
import { getWebAutoInstrumentations } from '@opentelemetry/auto-instrumentations-web';
import {OTLPTraceExporter} from "@opentelemetry/exporter-trace-otlp-proto";

export default defineNuxtPlugin({
  name: 'opentelemetry',
  async setup(_){
    const config = useRuntimeConfig();
    const { otelExporterOtlpEndpoint: otlpUrl, otelExporterOtlpHeaders: headers, otelResourceAttributes: resourceAttributes, otelServiceName: serviceName } = config.public;
    if (otlpUrl && headers && resourceAttributes && serviceName) {
      initializeTelemetry({
        otlpUrl,
        headers,
        resourceAttributes,
        serviceName});
  }
}})

interface InitializeTelemetryParams {
  otlpUrl: string;
  headers: string;
  resourceAttributes: string;
  serviceName: string;
}

function initializeTelemetry({otlpUrl, headers, resourceAttributes, serviceName}: InitializeTelemetryParams) {
  const otlpOptions = {
    url: `${otlpUrl}/v1/traces`,
    headers: parseDelimitedValues(headers)
  };
  
  const attributes = parseDelimitedValues(resourceAttributes);
  if (serviceName) {
    attributes[ATTR_SERVICE_NAME] = serviceName;
  }

  const provider = new WebTracerProvider({
    resource: new Resource(attributes),
    spanProcessors: [
      new SimpleSpanProcessor(new ConsoleSpanExporter()),
      new SimpleSpanProcessor(new OTLPTraceExporter(otlpOptions)),
    ],
  });

  provider.register({
    // Changing default contextManager to use ZoneContextManager - supports asynchronous operations - optional
    contextManager: new ZoneContextManager(),
  });

  registerInstrumentations({
    instrumentations: getWebAutoInstrumentations({
      "@opentelemetry/instrumentation-fetch": {
          propagateTraceHeaderCorsUrls: [new RegExp(`\\/api\\/*`)],
      }
    }),
  });
}

export function parseDelimitedValues(s: string): Record<string, string> {
  const headers = s.split(",");
  const result: Record<string, string> = {};

  headers.forEach((header) => {
    const [key, value] = header.split("=");
    if (key && value) {
      result[key.trim()] = value.trim();
    }
  });

  return result;
}