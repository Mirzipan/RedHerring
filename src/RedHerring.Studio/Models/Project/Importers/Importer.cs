﻿namespace RedHerring.Studio.Models.Project.Importers;

public interface Importer
{
    ImporterSettings CreateSettings();
    ImporterResult   Import(Stream stream, ImporterSettings settings, string resourcePath, CancellationToken cancellationToken);
}